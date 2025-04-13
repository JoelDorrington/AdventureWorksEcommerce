import { Component, signal, OnInit } from '@angular/core';
import { ProductsService, ProductPartial } from '../../services/products.service';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { CurrencyPipe } from '@angular/common';
import { FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { Subscription, fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs';

interface QueryFormValues {
  search: string;
  category: number;
  sort: string;
  reverse: boolean;
}

@Component({
  selector: 'app-products',
  standalone: true,
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss',
  imports: [RouterLink, CurrencyPipe, ReactiveFormsModule]
})

export class ProductsComponent implements OnInit {
  private scrollSubscription: Subscription | null = null;
  private scrollContainer: HTMLDivElement | null = null;

  loading = signal<boolean>(false);
  hasMoreProducts = signal<boolean>(true);

  products = signal<ProductPartial[]>([]);
  categories = signal<{value: number, label: string}[]>([]);

  queryControls: FormGroup = new FormGroup({
    category: new FormControl(''),
    search: new FormControl(''),
    sort: new FormControl('name'),
    reverse: new FormControl(false)
  });

  pageNumber: number = 1;
  pageLength: number = 50;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private productsService: ProductsService,
  ) { }

  ngOnInit(): void {
    this.scrollContainer = document.querySelector('.products-container');
    if (this.scrollContainer) {
      this.scrollSubscription = fromEvent(this.scrollContainer, 'scroll')
        .pipe(debounceTime(200))
        .subscribe(() => {
          this.onScroll();
        });
    }

    this.productsService.getProductCategories()
      .subscribe((categories) => {
        this.categories.set(
          categories.map(_ => ({
            value: _.productSubcategoryID,
            label: `${_.categoryName} - ${_.name}`
        })));
      });

    // Sync form values with query parameters
    this.route.queryParams.subscribe((params) => {
      this.queryControls.patchValue({
        search: params['search'] || '',
        category: params['category'] || '',
        sort: params['sort'] || 'name',
        reverse: params['reverse'] === 'true' // Convert string to boolean
      });
    });

    // Update query parameters when form values change
    this.queryControls.valueChanges
      .pipe(debounceTime(200))
      .subscribe((formValues) => {
        this.products.set([]);
        this.hasMoreProducts.set(true);
        this.pageNumber = 1;
        this.router.navigate([], {
          relativeTo: this.route,
          queryParams: formValues,
          queryParamsHandling: 'merge' // Merge with existing query params
        });

        this.fetchProducts(formValues);
      })

    this.fetchProducts(this.queryControls.value);
  }

  onScroll(): void {
    if (!this.scrollContainer) return;
    const target = this.scrollContainer;
    const scrollPosition = target.scrollTop + target.clientHeight;
    const scrollHeight = target.scrollHeight;

    // Trigger fetch when scrolled near the bottom
    if (scrollPosition >= scrollHeight - 100) {
      this.pageNumber++;
      this.fetchProducts(this.queryControls.value);
    }
  }

  fetchProducts(formValues: QueryFormValues): void {
    if (this.loading() || !this.hasMoreProducts()) return;
    this.loading.set(true);

    this.productsService.getProducts({
      category: formValues.category,
      search: formValues.search,
      page: this.pageNumber,
      length: this.pageLength,
      sort: formValues.sort,
      reverse: formValues.reverse
    }).subscribe(response => {
      this.products.set(this.products().concat(response));
      this.hasMoreProducts.set(response.length === this.pageLength);
      this.loading.set(false);
    });
  }

  ngOnDestroy(): void {
    // Unsubscribe to prevent memory leaks
    if (this.scrollSubscription) {
      this.scrollSubscription.unsubscribe();
    }
  }
  
}

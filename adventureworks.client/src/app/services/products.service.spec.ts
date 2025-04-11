import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductPartial, ProductsService } from './products.service';

describe('ProductsService', () => {
  let service: ProductsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProductsService],
    });
    service = TestBed.inject(ProductsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch products successfully', () => {
    const mockProducts: ProductPartial[] = [
      { id: 1, name: 'Product A', price: '100.00', imgUrl: '' },
      { id: 2, name: 'Product B', price: '200.00' , imgUrl: '' },
    ];

    service.getProducts({}).subscribe((products) => {
      expect(products.length).toBe(2);
      expect(products).toEqual(mockProducts);
    });

    const req = httpMock.expectOne('/api/products');
    expect(req.request.method).toBe('GET');
    req.flush(mockProducts);
  });

  it('should handle error when fetching products', () => {
    const errorMessage = 'Failed to load products';

    service.getProducts({}).subscribe(
      () => fail('Expected an error, not products'),
      (error) => {
        expect(error).toBe(errorMessage);
      }
    );

    const req = httpMock.expectOne('/api/products');
    req.flush(errorMessage, { status: 500, statusText: 'Server Error' });
  });
});

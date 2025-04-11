import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpErrorResponse } from '@angular/common/http';
import { catchError, retryWhen, delay, take } from 'rxjs/operators';
import { throwError } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class RetryInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    return next.handle(req).pipe(
      retryWhen(errors =>
        errors.pipe(
          delay(2000), // Wait 2 seconds before retrying
          take(5) // Retry up to 5 times
        )
      ),
      catchError((error: HttpErrorResponse) => {
        console.error('Backend not ready:', error);
        return throwError(error);
      })
    );
  }
}

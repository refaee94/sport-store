import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Repository } from '../models/repository';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthenticationService {
  constructor(private repo: Repository, private router: Router) {}
  name: string;
  password: string;
  callbackUrl: string;
  jwtHelper = new JwtHelperService();
  login(): Observable<any> {
    return this.repo.login(this.name, this.password).pipe(
      map(response => {
        if (response.ok) {
          console.log(response)
          localStorage.setItem('token', JSON.parse(response._body).token);
          this.router.navigateByUrl(this.callbackUrl || '/admin/overview');
        }
      }),
      catchError(e => {console.log(e);
        return of(null);
      })
    );
  }
  logout() {
    this.repo.logout();
    this.router.navigateByUrl('/login');
  }
  loggedIn() {
    return !this.jwtHelper.isTokenExpired(localStorage.getItem('token'));
  }
}

import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
  Router,
  Route
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from './Authentication.service';

@Injectable()
export class AuthenticationGuard implements CanActivateChild {
  /**
   *
   */
  constructor(
    private authService: AuthenticationService,
    private router: Router
  ) {}
  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean | Observable<boolean> | Promise<boolean> {
    if (this.authService.loggedIn()) {
      return true;
    } else {
      this.authService.callbackUrl = '/admin/' ;
      this.router.navigateByUrl('/login');
      return false;
    }
  }
}

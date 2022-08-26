import { Injectable } from '@angular/core';
import { EVENT_MANAGER_PLUGINS } from '@angular/platform-browser';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { elementAt, map } from 'rxjs/operators';

import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private acountService:AccountService) {}
  canActivate(): Observable<boolean> {
    return this.acountService.currentUser$.pipe(
      map(user => {
        if(user) return true;

      })
    );
  }

}

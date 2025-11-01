import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { EconomyApiClient } from './economy-api-client';
import { User } from '../models/User';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    userLoggedInChanged: BehaviorSubject<boolean>;

    constructor(private api: EconomyApiClient, private router: Router) {
        this.userLoggedInChanged = new BehaviorSubject(false);
    }

    login(username: string, password: string) {
        return this.api
            .post<any>(`users/authenticate`, { username, password })
            .pipe(
                map(user => {
                    // login successful if there's a jwt token in the response
                    if (user && user.token) {
                        // store user details and jwt token in local storage to keep user logged in between page refreshes
                        localStorage.setItem(
                            'currentUser',
                            JSON.stringify(user)
                        );
                        this.userLoggedInChanged.next(true);
                    }

                    return user;
                })
            );
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.router.navigate(['/login']);
        this.userLoggedInChanged.next(false);
    }

    get currentUser(): User {
        let currentUser = JSON.parse(localStorage.getItem('currentUser'));
        if (currentUser && currentUser.token) {
            return currentUser;
        }
        return null;
    }
}

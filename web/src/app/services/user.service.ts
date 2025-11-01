import { Injectable } from '@angular/core';
import { User } from '../models/User';
import { EconomyApiClient } from './economy-api-client';

@Injectable({ providedIn: 'root' })
export class UserService {
    constructor(private api: EconomyApiClient) {}

    getAll() {
        return this.api.get<User[]>('users');
    }
}

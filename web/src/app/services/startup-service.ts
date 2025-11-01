import { AppSettings } from '../models/AppSettings';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class StartupService {
    settings: AppSettings;

    constructor() {
        this.settings = new AppSettings();
        this.settings.apiUrl = 'http://localhost:56791/api';
    }

    getApiUrl(): string {
        return this.settings.apiUrl;
    }
}

import { Injectable } from '@angular/core';
import { EconomyApiClient } from './economy-api-client';
import { Observable } from 'rxjs';
import { AircraftDTO } from '../dto/AircraftDTO';

@Injectable({
    providedIn: 'root'
})
export class AircraftService {
    constructor(private economyApiClient: EconomyApiClient) {}

    getAircraftByIcao(icao: string): Observable<AircraftDTO[]> {
        return this.economyApiClient.get(`aircraft/${icao}`);
    }
}

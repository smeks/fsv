import { Injectable } from '@angular/core';
import { PlayerDTO } from '../dto/PlayerDTO';
import { Observable } from 'rxjs';
import { EconomyApiClient } from './economy-api-client';

@Injectable({
    providedIn: 'root'
})
export class PlayerService {
    constructor(private economyApiClient: EconomyApiClient) {}

    getPlayer(): Observable<PlayerDTO> {
        return this.economyApiClient.get<PlayerDTO>('player');
    }

    rent(aircraftId: string): Observable<PlayerDTO> {
        return this.economyApiClient.put<PlayerDTO>(
            `player/rent/${aircraftId}`,
            null
        );
    }

    release(aircraftId: string): Observable<PlayerDTO> {
        return this.economyApiClient.delete<PlayerDTO>(`player/rent`);
    }

    addJob(jobId: string): Observable<PlayerDTO> {
        return this.economyApiClient.put<PlayerDTO>(
            `player/job/${jobId}`,
            null
        );
    }

    removeJob(jobId: string): Observable<PlayerDTO> {
        return this.economyApiClient.delete<PlayerDTO>(`player/job/${jobId}`);
    }
}

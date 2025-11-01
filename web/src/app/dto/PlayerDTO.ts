import { JobDTO } from './JobDTO';
import { AircraftDTO } from './AircraftDTO';

export class PlayerDTO {
    id: string;
    userName: string;
    jobs: JobDTO[];
    money: number;
    departingJobs: JobDTO[];
    enrouteJobs: JobDTO[];
    rentedAircraft: AircraftDTO;
}

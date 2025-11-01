export class JobDTO {
    id: string;
    description: string;
    fromIcao: string;
    toIcao: string;
    amount: number;
    amountUom: string;
    type: string;
    weight: number;
    weightUom: string;
    isAssigned: boolean;
}

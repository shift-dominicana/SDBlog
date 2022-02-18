import internal from "stream";

export class Base {
    Id: number;
    CreatedBy: internal;
    CreatedDate: Date;
    ModifiedDate: string;
    ModifiedBy: internal;
    Status: string;
    isDeleted: boolean;
}
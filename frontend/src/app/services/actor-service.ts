import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MyConfig } from '../my-config';

@Injectable({
    providedIn: 'root'
})
export class ActorService {
    constructor(private http: HttpClient) { }

    addActor(actor: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Actors/AddActor`, actor);
    }

    deleteActor(id: number): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Actors/DeleteActorById`, { id });
    }

    updateActor(actor: any): Observable<any> {
        return this.http.post(`${MyConfig.APIurl}/api/Actors/UpdateActor`, actor);
    }

    getActorById(id: number): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Actors/GetActorById?id=${id}`);
    }

    getAllActors(): Observable<any> {
        return this.http.get(`${MyConfig.APIurl}/api/Actors/GetAllActors`);
    }
}
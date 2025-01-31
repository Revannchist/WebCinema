import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserCreateDto } from '../models/dto/user-create-dto';
import { UserDisplayDto } from '../models/dto/user-display-dto';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = `${environment.apiUrl}/api/Users`;

  constructor(private http: HttpClient) { }

  addUser(user: UserCreateDto): Observable<UserDisplayDto> {
    return this.http.post<UserDisplayDto>(`${this.baseUrl}/AddUser`, user);
  }

  getAllUsers(): Observable<UserDisplayDto[]> {
    return this.http.get<UserDisplayDto[]>(`${this.baseUrl}/GetAllUsers`);
  }

  getUserById(id: number): Observable<UserDisplayDto> {
    return this.http.get<UserDisplayDto>(`${this.baseUrl}/GetUserById?id=${id}`);
  }

  updateUser(id: number, user: any): Observable<UserDisplayDto> {
    const updateData = {
      id: id,
      username: user.username,
      email: user.email,
      password: user.password,
      firstName: user.firstName,
      lastName: user.lastName,
      dateOfBirth: new Date(user.dateOfBirth).toISOString(),
      registrationTime: new Date().toISOString(),
      roleId: user.roleId,
      roles: {
        id: user.roleId,
        name: user.roleId === 2 ? "User" : "Admin"
      }
    };

    console.log('Service sending data:', updateData);
    
    return this.http.post<UserDisplayDto>(`${this.baseUrl}/UpdateUser?id=${id}`, updateData);
  }

  deleteUser(id: number): Observable<UserDisplayDto> {
    return this.http.post<UserDisplayDto>(`${this.baseUrl}/DeleteUserById?id=${id}`, id);
  }
}

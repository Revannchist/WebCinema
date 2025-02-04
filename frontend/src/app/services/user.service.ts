import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
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

  login(loginData: { username: string, password: string }): Observable<UserDisplayDto> {
    // Prvo dohvatimo sve korisnike
    return this.getAllUsers().pipe(
      map(users => {
        // Tražimo korisnika s odgovarajućim username-om
        const user = users.find(u => u.username === loginData.username);
        
        if (!user) {
          throw new Error('User not found');
        }
        
        // Ovdje bi inače bila prava provjera lozinke, ali za sad ćemo samo simulirati
        if (loginData.password !== user.password) {
          throw new Error('Invalid password');
        }
        
        return user;
      })
    );
  }

  createUser(userData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/AddUser`, userData);
  }

  getUsersPaged(page: number, pageSize: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/GetUsersPaged?page=${page}&pageSize=${pageSize}`);
  }

  getUsersPagedAndFiltered(page: number, pageSize: number, searchTerm: string = '') {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString())
      .set('searchTerm', searchTerm);

    return this.http.get<any>(`${this.baseUrl}/GetUsersPagedAndFiltered`, { params });
  }
}

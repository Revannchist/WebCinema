import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';

@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrl: './genres.component.css'
})
export class GenresComponent {
  constructor(private http:HttpClient){}
  public genres:any

  ngOnInit():void{
    this.http.get(MyConfig.APIurl + '/api/Genres/GetAllGenres').subscribe(x=>{
      this.genres = x;
      console.log(this.genres);
    })
  }
}
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrl: './test.component.css'
})
export class TestComponent {
  constructor(private http:HttpClient){}
  public countries:any
ngOnInit():void{
  console.log('kita');
  this.http.get(MyConfig.APIurl + '/api/Countries/GetAllCountries').subscribe(x=>{
    this.countries = x;
    console.log(this.countries);
  })
}
}
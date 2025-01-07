import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from '../../my-config';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrl: './countries.component.css'
})

export class CountriesComponent {
  constructor(private http:HttpClient){}
  public countries:any
  public countryToAdd:any={}
  public search:any
  public filteredCountries:any

  ngOnInit():void{
    this.http.get(MyConfig.APIurl + '/api/Countries/GetAllCountries').subscribe(x=>{
      this.countries = x;
      this.filteredCountries = x;
      console.log(this.countries);
  }
)
}

deleteCountry(country:any):void{
  this.http.post(MyConfig.APIurl + '/api/Countries/DeleteCountryById?id=' + country.id, [{}]).subscribe(
    (response)=>{
      console.log('Country delete response: ' + response);
    }
  )
}

clearModalTextBox():void{
  this.countryToAdd.name="";
}

addCountry():void{
  this.http.post(MyConfig.APIurl + '/api/Countries/AddCountry', {id:0, name:this.countryToAdd.name}).subscribe(
    (response)=>{
      console.log('Add country response: ' + response);
    }
  )
}

filterCountries():void{
  if(!this.search){
    this.filteredCountries=this.countries;
  }
  else{
  this.filteredCountries = this.countries.filter((country:any) => 
    country.name.toLowerCase().includes(this.search.toLowerCase())
  );
  }
}
}

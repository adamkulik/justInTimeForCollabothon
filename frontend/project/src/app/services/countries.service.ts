import { HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Country } from '../interfaces/country';
import { Enviroment } from '../environment/enviroment';

@Injectable({
  providedIn: 'root'
})
export class CountriesService {
  private apiURL = Enviroment.apiURL;
  constructor(private http: HttpClient) { }
  
  getRandomNumber(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min)) + min;
  }
  randomNumber : number = this.getRandomNumber(1, 9001);
  randomNumberForMap : number = this.getRandomNumber(1, 20)
  
  addData(): Observable<Country[]> {
    return this.http.get<Country[]>(`${this.apiURL}/country-report?seed=${this.randomNumber}`);
  }

  getMap() {
    return this.http.get(`http://48.209.21.72:7654/api/Transactions/getSVGMap?seed=${this.randomNumberForMap}`, {responseType: 'text'});
  }
}

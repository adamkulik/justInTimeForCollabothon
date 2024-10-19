import { Component, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Color, ScaleType } from '@swimlane/ngx-charts';
import { Country } from '../../interfaces/country';
import { CountriesService } from '../../services/countries.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit{
  // Define the color scheme for the pie chart
  colorScheme: Color = {
    domain: ['#3A7E8A', '#D5DBB6', '#93C1B4', '#D6C18B'],
    name: 'My Color Scheme',
    selectable: false,
    group: ScaleType.Linear
  };

  countries : Country[] = [];
  isVisible = true;
  viewForPie: [number, number] = [500, 300];
  viewForBar: [number, number] = [400, 300];


  forGrahcsDataset: {
    name: string; 
    value: number;
  }[] =[] ;
  mainDataset: {
    country: string;
    numberOfTransactions: number;
    totalSum: number;
  }[] | undefined;
  constructor(private router: Router, private countrieServise : CountriesService) {
  
  }

  ngOnInit(): void {
    this.countrieServise.addData().subscribe(
      (response: Country[]) => {
        console.log('API Response:', response);
  
        // Map data for the charts
        this.forGrahcsDataset = response.map(country => ({
          name: country.Country,  
          value: country.TotalSum
        }));
  
        //getting mainData
        this.mainDataset = response.map(country => ({
          country: country.Country,
          numberOfTransactions: country.NumberOfTransactions,
          totalSum: country.TotalSum
        }));
      },
      (error) => {
        // Handle error response
        console.error('Error fetching country data:', error);
      }
    );
  }

  OpeningMap() {
    this.router.navigate(['map']);
  }
}

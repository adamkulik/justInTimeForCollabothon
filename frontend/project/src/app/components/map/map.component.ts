import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CountriesService } from '../../services/countries.service';
import { Country } from '../../interfaces/country';
import { HttpClient } from '@angular/common/http';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';


@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrl: './map.component.scss'
})
export class MapComponent implements OnInit{
  countries : Country[] = [];
  mainDataset: {
    country: string;
    numberOfTransactions: number;
    totalSum: number;
  }[] | undefined;
  constructor(private router : Router, private countrieServise : CountriesService, private http : HttpClient, public sanitizer: DomSanitizer) {
  }
  isVisible : boolean = true;
  disappearing() {
    this.isVisible = !this.isVisible;
  }

 svg!: SafeHtml;
 svgCode!: string;

 polandData=[{name: "Poland"}, {numberOfTransactions: 33345}, {totalSum: 435355} ];

  ngOnInit(): void {

    this.countrieServise.getMap().subscribe(
      (response: string) => {
        this.svgCode = response;  // Store the response in svgCode variable
        console.log(this.svgCode);
        
        // Sanitize the SVG and assign to the svg variable
        this.svg = this.sanitizer.bypassSecurityTrustHtml(this.svgCode);
      }, 
      (error: any) => {
        console.error('Error fetching SVG:', error);
      }
    );

    this.countrieServise.addData().subscribe(
      (response: Country[]) => {
        console.log('API Response:', response);
  
        // Map data for the mainDataset
        this.mainDataset = response.map(country => ({
          country: country.Country,
          numberOfTransactions: country.NumberOfTransactions,
          totalSum: country.TotalSum
        }));
  // // Find the data for Poland
  // const polandData = this.mainDataset.find(item => item.country === 'Poland');

  // // If Poland data is found, assign it to this.polandData
  // if (polandData) {
  //   this.polandData2.push(polandData); // Now it's stored in an array
  //   console.log('Poland Data:', this.polandData2); // Log to verify
  // } else {
  //   console.log('No data found for Poland');
  // }

      },
      (error) => {
        // Handle error response here
        console.error('Error fetching country data:', error);
      }
    );
  }


  openHome() {
    this.router.navigate(['']);
  }
}

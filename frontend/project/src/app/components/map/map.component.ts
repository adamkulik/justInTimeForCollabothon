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

 svg!: SafeHtml;
 svgCode!: string;
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

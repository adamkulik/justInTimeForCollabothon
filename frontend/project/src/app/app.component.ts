import { Component } from '@angular/core';
import { Color, ScaleType } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'project';
   // Sample data for the chart
   single = [
    {
      name: 'Germany',
      value: 8940000
    },
    {
      name: 'USA',
      value: 5000000
    },
    {
      name: 'France',
      value: 7200000
    }
  ];
  // Define the color scheme correctly
  colorScheme: Color = {
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA'], // Array of colors
    name: 'My Color Scheme', // Optional: specify a name for your color scheme
    selectable: false,        // Optional: whether the colors are selectable
    group: ScaleType.Linear       // Specify a group value as needed
  };

  view: number[] = [700, 400];

  onSelect(event: any) {
    console.log(event);
  }
}

import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { LayoutComponent } from '../../components/layout/layout.component';
import { HomeComponent } from '../../components/home/home.component';
import { MapComponent } from '../../components/map/map.component';
import { CommonModule } from '@angular/common';


const routes: Routes = [{
  path: '', component: LayoutComponent, children: [
    {path: '', component: HomeComponent},
    {path: 'map', component:MapComponent}]

}]

@NgModule({
  declarations: [HomeComponent, MapComponent],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes),
    NgxChartsModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class LayoutRoutingModule { }

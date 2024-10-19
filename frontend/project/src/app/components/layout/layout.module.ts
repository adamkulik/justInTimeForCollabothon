import {  NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LayoutComponent } from './layout.component';
import { SharedModule } from '../../shared/shered/shered.module';
import { LayoutRoutingModule } from '../../shared/layout-routing/layout-routing.module';



@NgModule({
  declarations: [LayoutComponent],
  imports: [
    CommonModule,
    RouterModule,
    SharedModule,
    LayoutRoutingModule
  ]
})
export class LayoutModule { }
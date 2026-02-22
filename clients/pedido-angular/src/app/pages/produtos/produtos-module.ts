import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { ProdutosRoutingModule } from './produtos-routing-module';
import { List } from './list/list';
import { Form } from './form/form';
import { FormField } from "@angular/forms/signals";

@NgModule({
  declarations: [
    List,
    Form
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    ProdutosRoutingModule,
    FormField
  ]
})
export class ProdutosModule { }

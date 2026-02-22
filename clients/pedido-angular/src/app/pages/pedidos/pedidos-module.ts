import { NgModule, Pipe } from '@angular/core';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { PedidosRoutingModule } from './pedidos-routing-module';
import { List } from './list/list';
import { Form } from './form/form';
import { Ver } from './ver/ver';
import { FormField } from '@angular/forms/signals';

@NgModule({
  declarations: [
    List,
    Form,
    Ver
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    PedidosRoutingModule,
    FormField,
    CurrencyPipe
  ]
})
export class PedidosModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { List as PedidoList } from './list/list';
import { Form as PedidoForm } from './form/form';
import { Ver as VerForm } from './ver/ver';

const routes: Routes = [
  { path: '', component: PedidoList },
  { path: 'novo', component: PedidoForm },
  { path: 'editar/:id', component: PedidoForm },
  { path: 'ver/:id', component: VerForm }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PedidosRoutingModule { }

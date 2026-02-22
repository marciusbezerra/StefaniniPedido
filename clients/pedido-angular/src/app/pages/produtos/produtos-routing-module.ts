import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { List as ProdutoList } from './list/list';
import { Form as ProdutoForm } from './form/form';

const routes: Routes = [
  { path: '', component: ProdutoList },
  { path: 'novo', component: ProdutoForm },
  { path: 'editar/:id', component: ProdutoForm }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProdutosRoutingModule { }

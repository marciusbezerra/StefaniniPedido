import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/pedidos', pathMatch: 'full' },
  {
    path: 'pedidos',
    loadChildren: () => import('./pages/pedidos/pedidos-module').then(m => m.PedidosModule)
  },
  {
    path: 'produtos',
    loadChildren: () => import('./pages/produtos/produtos-module').then(m => m.ProdutosModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

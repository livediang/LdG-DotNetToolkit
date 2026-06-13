import { Routes } from '@angular/router';
import { ListBooksPage } from './Features/ObjBooks/Pages/list-books-page/list-books-page';
import { CreateBooksPage } from './Features/ObjBooks/Pages/create-books-page/create-books-page';

export const routes: Routes = [
    {path:"", component:ListBooksPage},
    {path:"create", component:CreateBooksPage}
];

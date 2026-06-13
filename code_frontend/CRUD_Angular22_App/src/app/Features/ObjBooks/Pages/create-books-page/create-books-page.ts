import { Component, inject, OnInit, signal } from '@angular/core';
import { GenresService } from '../../../ObjGenres/Services/genres-service';
import { BooksService } from '../../Services/books-service';
import { Title } from '@angular/platform-browser';
import { BooksModel } from '../../Models/books-model';
import { GetGenreResponse } from '../../../../Shared/Interfaces/get-genre-response';
import { form, required, validate, FormField } from '@angular/forms/signals';
import { CreateBookRequest } from '../../../../Shared/Interfaces/create-book-request';
import Swal from 'sweetalert2';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-create-books-page',
  imports: [FormField, RouterLink],
  templateUrl: './create-books-page.html',
  styleUrl: './create-books-page.css',
})
export class CreateBooksPage implements OnInit {
  private genresService = inject(GenresService)
  private booksService = inject(BooksService)

  genre = signal<GetGenreResponse[]>([]);

  bookModel = signal<BooksModel>({
    BookId:"0",
    Title:"",
    Author:"",
    YearOfRead:"",
    GenreId:"0"
  })

  booksForm = form(this.bookModel,(schemaPath)=>{
    required(schemaPath.Title,{message:"Title is required"});
    required(schemaPath.Author,{message:"Author is required"});
    required(schemaPath.YearOfRead,{message:"Date is required"});
    validate(schemaPath.GenreId,({value})=>{
      if(value().match("0")) return {kind:"equals", message:"Genre is required"};
      return null;
    })
  })

  ngOnInit(): void {
    this.genresService.getAll().subscribe({
      next: response=> {
        this.genre.set(response)
      },
      error:(e) => {console.log(e.error)}
    })
  }

  onSave(){
    const request:CreateBookRequest = {
      title: this.booksForm.Title().value(),
      author: this.booksForm.Author().value(),
      yearOfRead: this.booksForm.YearOfRead().value(),
      genreId: Number(this.booksForm.GenreId().value())
    }

    this.booksService.create(request).subscribe({
      next: response=> {
        this.resetForm();
        Swal.fire({
          text:"Regitered Book",
          icon:"success"
        })
      },
      error:(e) => {console.log(e.error)}
    })
  }

  private resetForm(): void {
    this.bookModel.set({
      BookId:"0",
      Title:"",
      Author:"",
      YearOfRead:"",
      GenreId:"0"
    })
    this.booksForm().reset();
  }
}

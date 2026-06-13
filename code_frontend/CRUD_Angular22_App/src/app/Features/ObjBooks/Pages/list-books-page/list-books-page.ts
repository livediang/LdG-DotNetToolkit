import { Component, inject, OnInit, signal, TemplateRef } from '@angular/core';
import { GenresService } from '../../../ObjGenres/Services/genres-service';
import { BooksService } from '../../Services/books-service';
import { GetGenreResponse } from '../../../../Shared/Interfaces/get-genre-response';
import { GetBookResponse } from '../../../../Shared/Interfaces/get-book-response';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { BooksModel } from '../../Models/books-model';
import { form, FormField, required, validate } from '@angular/forms/signals';
import { UpdateBookRequest } from '../../../../Shared/Interfaces/update-book-request';
import Swal from 'sweetalert2';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-list-books-page',
  imports: [RouterLink, FormField],
  templateUrl: './list-books-page.html',
  styleUrl: './list-books-page.css',
})
export class ListBooksPage implements OnInit {
  private genresService = inject(GenresService)
  private booksService = inject(BooksService)
  private modalService = inject(NgbModal)

  genre = signal<GetGenreResponse[]>([]); 
  book = signal<GetBookResponse[]>([]);

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

  getGenres(){
    this.genresService.getAll().subscribe({
      next: response=> {
        this.genre.set(response)
      },
      error:(e) => {console.log(e.error)}
    })
  }

  getBooks(){
    this.booksService.getAll().subscribe({
      next: response=> {
        this.book.set(response)
      },
      error:(e) => {console.log(e.error)}
    })
  }

  ngOnInit(): void {
    this.getGenres()
    this.getBooks()
  }

  openModal(modalHtml:TemplateRef<any>, Id:number){
    this.booksService.getById(Id).subscribe({
      next: response => {
        const YearOfRead = new Date(response.yearOfRead)
        this.bookModel.set({
          BookId: response.bookId.toString(),
          Title: response.title.toString(),
          Author: response.author.toString(),
          YearOfRead: YearOfRead.toISOString().split("T")[0],
          GenreId: response.genreId.toString()
        })
      },
      error:(e) => {console.log(e.error)}
    })
    this.modalService.open(modalHtml,{size:"lg"})
  }

  saveChanges(){
    const request : UpdateBookRequest ={
      bookId: Number(this.booksForm.BookId().value()),
      title: this.booksForm.Title().value(),
      author: this.booksForm.Author().value(),
      yearOfRead: this.booksForm.YearOfRead().value(),
      genreId: Number(this.booksForm.GenreId().value())
    }

    this.booksService.update(request).subscribe({
      next: response=> {
        Swal.fire({
          text:"Updated Book",
          icon:"success"
        })
        this.modalService.dismissAll();
        this.getBooks();
      },
      error:(e) => {console.log(e.error)}
    })
  }

  delete(id:number) {
    Swal.fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Yes, delete it!"
    }).then((result) => {
      if (result.isConfirmed) {
        this.booksService.delete(id).subscribe({
          next: response => {
            console.log(response)
            Swal.fire({
              text:"Remove Book",
              icon:"success"
            })
            this.getBooks()
          },
          error:(e) => {console.log(e.error)}
        })
      }
    });
  }
}

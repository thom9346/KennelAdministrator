import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Person } from '../models/person';
import { Picture } from '../models/picture';


@Injectable({
  providedIn: 'root'
})
export class PeopleService {

  constructor(private httpClient : HttpClient) { }

  BASEURL = 'http://localhost:5000/api/';


  postPeopleWithImage(formData: FormData){
    return this.httpClient.post(this.BASEURL + "people", formData)
  }
  putPerson(id:string, formData: FormData) {
    return this.httpClient.put<Person>(this.BASEURL + "people/" + id, formData);
  }
  getPeople(){
    return this.httpClient.get<Person[]>(this.BASEURL + "people");
  }

  getPerson(id: string) {
    return this.httpClient.get<Person>(this.BASEURL + "people/" + id);
  }

  deletePerson(id: string) {
    return this.httpClient.delete<Person>(this.BASEURL + "people/" + id);
  }

  getPictures(id: string) {
    return this.httpClient.get<Picture[]>(this.BASEURL + "pictures/" + id)
  }
}

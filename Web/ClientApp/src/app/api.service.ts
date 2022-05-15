import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';


const API_URL = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class APIService {

  constructor() { }
}

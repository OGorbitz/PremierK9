import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class NavMenuService {
  private isVisible: boolean = true;

  constructor(private router: Router) { }

  public visible(): boolean {
    let vis = !this.router.url.endsWith("login");
    return vis;
  }

}

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NavMenuService } from './nav-menu.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  constructor(private router: Router, public nav: NavMenuService) {}

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logOut = () => {
    localStorage.removeItem("jwt");
    this.router.navigate(["/login"]);
  }
}

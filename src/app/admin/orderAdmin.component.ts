import { Component } from '@angular/core';
import { Order } from '../models/order.model';
import { Repository } from '../models/repository';
@Component({
  templateUrl: 'orderAdmin.component.html'
})
export class OrderAdminComponent {
  constructor(private repo: Repository) {}
  get orders(): Order[] {
    return this.repo.orders;
  }
  markShipped(order: Order) {
    this.repo.shipOrder(order);
  }
}

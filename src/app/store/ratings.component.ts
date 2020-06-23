import { Component, Input } from '@angular/core';
import { Product } from '../models/product.model';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'store-ratings',
    templateUrl: 'ratings.component.html'
})
export class RatingsComponent {

    @Input()
    product: Product;

    get stars(): boolean[] {
        if (this.product != null && this.product.ratings != null) {
            const total = this.product.ratings.map(r => r.stars)
                .reduce((p, c) => p + c, 0);
            const count = Math.round(total / this.product.ratings.length);
            return Array(5).fill(false).map((value, index) => {
                return index < count;
            });
        } else {
            return [];
        }
    }
}

export interface Product {
    displayName: string;
    description: string;
    price: string;
    image: string;
    id:number;
}
export interface ProductListing {
    displayName: string; // Title of the product listing
    description: string; // Description of the product listing
    childProducts: Product[]; // Array of products under this listing
}
``

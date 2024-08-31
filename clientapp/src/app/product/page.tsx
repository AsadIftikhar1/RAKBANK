'use client'; 

import { Product } from '../interfaces/product';
import styles from '../css/products.module.css';
import { useState } from 'react';

const initialProducts: Product[] = [
    { id: 1, name: 'Product 1', description: 'This is a description of Product 1.', price: '$19.99', image: 'https://via.placeholder.com/150' },
    { id: 2, name: 'Product 2', description: 'This is a description of Product 2.', price: '$29.99', image: 'https://via.placeholder.com/150' },
    { id: 3, name: 'Product 3', description: 'This is a description of Product 3.', price: '$39.99', image: 'https://via.placeholder.com/150' },
];

const ProductsPage: React.FC = () => {
    const [products, setProducts] = useState<Product[]>(initialProducts);
    const [editingProduct, setEditingProduct] = useState<Product | null>(null);

    const startEditing = (product: Product) => {
        setEditingProduct(product);
    };

    const cancelEditing = () => {
        setEditingProduct(null);
    };

    const saveProduct = (updatedProduct: Product) => {
        setProducts(products.map(p => p.id === updatedProduct.id ? updatedProduct : p));
        setEditingProduct(null);
    };

    const deleteProduct = (id: number) => {
        setProducts(products.filter(p => p.id !== id));
        setEditingProduct(null);
    };

    const handleEditChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        if (editingProduct) {
            setEditingProduct({
                ...editingProduct,
                [e.target.name]: e.target.value,
            });
        }
    };

    const ProductCard: React.FC<{ product: Product }> = ({ product }) => (
        <article className={styles.productCard}>
            <img src={product.image} alt={product.name} className={styles.productImage} />
            <div className={styles.productInfo}>
                <h2 className={styles.productTitle}>{product.name}</h2>
                <p className={styles.productDescription}>{product.description}</p>
                <span className={styles.productPrice}>{product.price}</span>
                <div className={styles.buttonContainer}>
                    <button onClick={() => startEditing(product)}>Edit</button>
                    <button onClick={() => deleteProduct(product.id)} className={styles.deleteButton}>Delete</button>
                </div>
            </div>
        </article>
    );
    interface Props {
        editingProduct: Product | null;
        onSave: (updatedProduct: Product) => void;
        onCancel: () => void;
        onClose: () => void;
    }

    const EditProduct: React.FC = () => {
        if (!editingProduct) return null;

        return (
            <div className={styles.editContainer}>
                <h2>Edit Product</h2>
                <form onSubmit={(e) => { e.preventDefault(); if (editingProduct) saveProduct(editingProduct); }}>
                    <label>
                        Name:
                        <input type="text" name="name" value={editingProduct.name} onChange={handleEditChange} />
                    </label>
                    <label>
                        Description:
                        <textarea name="description" value={editingProduct.description} onChange={handleEditChange} />
                    </label>
                    <label>
                        Price:
                        <input type="text" name="price" value={editingProduct.price} onChange={handleEditChange} />
                    </label>
                    <button type="submit">Save</button>
                    <button type="button" className={styles.cancelButton} onClick={cancelEditing}>Cancel</button>
                </form>
            </div>
        );
    };

    return (
        <div>
            <main className={styles.main}>
                <section className={styles.productList}>
                    {products.map(product => (
                        <ProductCard key={product.id} product={product} />
                    ))}
                </section>
                <EditProduct />
            </main>
        </div>
    );
}

export default ProductsPage;

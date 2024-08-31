'use client';

import { Product } from '../interfaces/product';
import { ProductListing } from '../interfaces/product';
import styles from '../css/products.module.css';
import { useEffect, useState } from 'react';

const ProductsPage: React.FC = () => {
    const [products, setProducts] = useState<Product[]>([]);
    const [editingProduct, setEditingProduct] = useState<Product | null>(null);
    const BASE_URL = 'https://localhost:5000/'; // Define your base URL

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await fetch('https://localhost:5000/api/ProductsListing');
                if (!response.ok) throw new Error('Network response was not ok');
                const data: ProductListing[] = await response.json();
                
                if (data.length > 0) {
                    setProducts(data[0].childProducts);
                }
            } catch (error) {
                console.error('Failed to fetch products:', error);
            }
        };
        fetchProducts();
    }, []);

    const startEditing = (product: Product) => {
        setEditingProduct(product);
    };

    const cancelEditing = () => {
        setEditingProduct(null);
    };

    const saveProduct = async (updatedProduct: Product) => {
        try {
            const response = await fetch(`https://localhost:5000/api/ProductsListing/${updatedProduct.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(updatedProduct),
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            setProducts(products.map(p => p.id === updatedProduct.id ? updatedProduct : p));
            setEditingProduct(null);
        } catch (error) {
            console.error('Failed to update product:', error);
        }
    };

    const deleteProduct = async (id: number) => {
        try {
            const response = await fetch(`https://localhost:5000/api/ProductsListing/${id}`, {
                method: 'DELETE',
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            setProducts(products.filter(p => p.id !== id));
            setEditingProduct(null);
        } catch (error) {
            console.error('Failed to delete product:', error);
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        if (editingProduct) {
            setEditingProduct({
                ...editingProduct,
                [name]: value,
            });
        }
    };

    const ProductCard: React.FC<{ product: Product }> = ({ product }) => (
        <article className={styles.productCard} key={product.id}>
            <img 
                src={product.image ? `${BASE_URL}${product.image}` : '/default-image.jpg'} 
                alt={product.displayName} 
                className={styles.productImage} 
            />
            <div className={styles.productInfo}>
                <h2 className={styles.productTitle}>Name: {product.displayName}</h2>
                <p className={styles.productDescription}>Description: {product.description}</p>
                <span className={styles.productPrice}>Price: {product.price}</span>
                <div className={styles.buttonContainer}>
                    <button onClick={() => startEditing(product)}>Edit</button>
                    <button onClick={() => deleteProduct(product.id)} className={styles.deleteButton}>Delete</button>
                </div>
            </div>
        </article>
    );

    const EditProduct: React.FC = () => {
        if (!editingProduct) return null;

        return (
            <div className={styles.editContainer}>
                <h2>Edit Product</h2>
                <form onSubmit={(e) => { e.preventDefault(); if (editingProduct) saveProduct(editingProduct); }}>
                    <label>
                        Name:
                        <input 
                            type="text" 
                            name="displayName" 
                            value={editingProduct.displayName} 
                            onChange={handleInputChange} 
                        />
                    </label>
                    <label>
                        Description:
                        <textarea 
                            name="description" 
                            value={editingProduct.description} 
                            onChange={handleInputChange} 
                        />
                    </label>
                    <label>
                        Price:
                        <input 
                            type="text" 
                            name="price" 
                            value={editingProduct.price} 
                            onChange={handleInputChange} 
                        />
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
};

export default ProductsPage;

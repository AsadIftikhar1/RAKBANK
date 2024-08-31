'use client';

import { Product } from '../interfaces/product';
import { ProductListing } from '../interfaces/product';
import styles from '../css/products.module.css';
import { useEffect, useState } from 'react';
import EditProduct from './editproduct'; // Import the new component

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

                console.log('Fetched data:', data);

                if (data.length > 0 && data[0].childProducts) {
                    setProducts(data[0].childProducts);
                } else {
                    console.error('Unexpected data structure:', data);
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
            console.log(updatedProduct);
            const response = await fetch(`https://localhost:5000/api/ProductsListing/UpdateProduct/${updatedProduct.id}`, {
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
            const url = `https://localhost:5000/api/ProductsListing/DeleteProduct/${id}`;
            console.log('Delete URL:', url);
            const response = await fetch(url, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                },
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            setProducts(products.filter(p => p.id !== id));
        } catch (error) {
            console.error('Failed to delete product:', error);
        }
    };

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;

        if (editingProduct) {
            setEditingProduct(prevProduct => ({
                ...prevProduct,
                [name]: value
            }) as Product);  // Ensure TypeScript understands the type correctly
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
                <h2 className={styles.productTitle}>{product.displayName}</h2>
                <p className={styles.productDescription}>{product.description}</p>
                <span className={styles.productPrice}>{product.price}</span>
                <div className={styles.buttonContainer}>
                    <button onClick={() => startEditing(product)}>Edit</button>
                    <button onClick={() => deleteProduct(product.id)} className={styles.deleteButton}>Delete</button>
                </div>
            </div>
        </article>
    );

    return (
        <div>
            <main className={styles.main}>
                <section className={styles.productList}>
                    {products.map(product => (
                        <ProductCard key={product.id} product={product} />
                    ))}
                </section>
                <EditProduct 
                    editingProduct={editingProduct}
                    handleInputChange={handleInputChange}
                    saveProduct={saveProduct}
                    cancelEditing={cancelEditing}
                />
            </main>
        </div>
    );
};

export default ProductsPage;

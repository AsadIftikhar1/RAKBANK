'use client';

import { useState } from 'react';
import { Product } from '../interfaces/product';
import styles from '../css/createproduct.module.css';
import { useRouter } from 'next/navigation';

interface CreateProductProps {
    showCreateForm: boolean;
    setShowCreateForm: (show: boolean) => void;
    setProducts: React.Dispatch<React.SetStateAction<Product[]>>;
}

const CreateProduct: React.FC<CreateProductProps> = ({ showCreateForm, setShowCreateForm, setProducts }) => {
    const [newProduct, setNewProduct] = useState<Partial<Product>>({
        displayName: '',
        description: '',
        price: '',
        image: ''
    });

    const router = useRouter(); // Correct usage of useRouter

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setNewProduct(prev => ({
            ...prev,
            [name]: value,
        }));
    };

    const createProduct = async (product: Partial<Product>) => {
        try {
            const response = await fetch('https://localhost:5000/api/ProductsListing/CreateProduct', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(product),
            });

            if (!response.ok) {
                throw new Error('Network response was not ok');
            }

            const newProduct: Product = await response.json();
            setProducts(prev => [...prev, newProduct]);

            setNewProduct({
                displayName: '',
                description: '',
                price: '',
                image: ''
            });
            setShowCreateForm(false);
            router.push('/product'); // Redirect to product listing page
        } catch (error) {
            console.error('Failed to create product:', error);
        }
    };

    if (!showCreateForm) return null;

    return (
        <div className={styles.createContainer}>
            <h2>Create New Product</h2>
            <form onSubmit={(e) => {
                e.preventDefault();
                createProduct({
                    displayName: newProduct.displayName || '',
                    description: newProduct.description || '',
                    price: newProduct.price || '',
                    image: newProduct.image || ''
                });
            }}>
                <label>
                    Name:
                    <input
                        type="text"
                        name="displayName"
                        value={newProduct.displayName || ''}
                        onChange={handleInputChange}
                    />
                </label>
                <label>
                    Description:
                    <textarea
                        name="description"
                        value={newProduct.description || ''}
                        onChange={handleInputChange}
                    />
                </label>
                <label>
                    Price:
                    <input
                        type="text"
                        name="price"
                        value={newProduct.price || ''}
                        onChange={handleInputChange}
                    />
                </label>
                <button type="submit">Create</button>
            </form>
        </div>
    );
};

export default CreateProduct;

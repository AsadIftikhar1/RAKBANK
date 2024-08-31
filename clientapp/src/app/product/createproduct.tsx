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
    const [imageFile, setImageFile] = useState<File | null>(null);
    const [errors, setErrors] = useState<{ [key: string]: string }>({});
    const router = useRouter();

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
        setNewProduct(prev => ({
            ...prev,
            [name]: value,
        }));
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            setImageFile(file);
            const reader = new FileReader();
            reader.onloadend = () => {
                setNewProduct(prev => ({
                    ...prev,
                    image: (reader.result as string).split(',')[1] // Only the Base64 data, excluding the `data:image/png;base64,` prefix
                }));
            };
            reader.readAsDataURL(file);
        }
    };

    const validateForm = () => {
        const newErrors: { [key: string]: string } = {};
        if (!newProduct.displayName) newErrors.displayName = 'Product name is required.';
        if (!newProduct.description) newErrors.description = 'Description is required.';
        if (!newProduct.price || isNaN(Number(newProduct.price))) newErrors.price = 'Valid price is required.';
        return newErrors;
    };

    const createProduct = async () => {
        const validationErrors = validateForm();
        if (Object.keys(validationErrors).length > 0) {
            setErrors(validationErrors);
            return;
        }

        try {
            const formData = new FormData();
            formData.append('displayName', newProduct.displayName || '');
            formData.append('description', newProduct.description || '');
            formData.append('price', newProduct.price || '');
            if (imageFile) {
                formData.append('image', imageFile);
            }
    
            const response = await fetch('https://localhost:5000/api/ProductsListing/CreateProduct', {
                method: 'POST',
                body: formData,
            });
    
            if (!response.ok) {
                const errorText = await response.text();
                throw new Error(`Network response was not ok: ${errorText}`);
            }
    
            const createdProduct: Product = await response.json();
            setProducts(prev => [...prev, createdProduct]);
    
            setNewProduct({
                displayName: '',
                description: '',
                price: '',
                image: ''
            });
            setImageFile(null);
            setShowCreateForm(false);
            router.push('/product');
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
                createProduct();
            }}>
                <label>
                    Name:
                    <input
                        type="text"
                        name="displayName"
                        value={newProduct.displayName || ''}
                        onChange={handleInputChange}
                        required
                    />
                    {errors.displayName && <p className={styles.error}>{errors.displayName}</p>}
                </label>
                <label>
                    Description:
                    <textarea
                        name="description"
                        value={newProduct.description || ''}
                        onChange={handleInputChange}
                        required
                    />
                    {errors.description && <p className={styles.error}>{errors.description}</p>}
                </label>
                <label>
                    Price:
                    <input
                        type="text"
                        name="price"
                        value={newProduct.price || ''}
                        onChange={handleInputChange}
                        required
                        pattern="^\d+(\.\d{1,2})?$"
                        title="Enter a valid price (e.g., 10 or 10.99)"
                    />
                    {errors.price && <p className={styles.error}>{errors.price}</p>}
                </label>
                <label>
                    Upload Product Image:
                    <input
                        type="file"
                        id="imageFile"
                        name="imageFile"
                        accept="image/*"
                        onChange={handleFileChange}
                    />
                </label>
                <button type="submit">Create</button>
            </form>
        </div>
    );
};

export default CreateProduct;

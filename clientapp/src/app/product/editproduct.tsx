'use client';

import React from 'react';
import { Product } from '../interfaces/product';
import styles from '../css/products.module.css';

interface EditProductProps {
    editingProduct: Product | null;
    handleInputChange: (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => void;
    saveProduct: (updatedProduct: Product) => void;
    cancelEditing: () => void;
}

const EditProduct: React.FC<EditProductProps> = ({ editingProduct, handleInputChange, saveProduct, cancelEditing }) => {
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
                        value={editingProduct.displayName || ''} 
                        onChange={handleInputChange} 
                    />
                </label>
                <label>
                    Description:
                    <textarea 
                        name="description" 
                        value={editingProduct.description || ''} 
                        onChange={handleInputChange} 
                    />
                </label>
                <label>
                    Price:
                    <input 
                        type="text" 
                        name="price" 
                        value={editingProduct.price || ''} 
                        onChange={handleInputChange} 
                    />
                </label>
                <button type="submit">Save</button>
                <button type="button" className={styles.cancelButton} onClick={cancelEditing}>Cancel</button>
            </form>
        </div>
    );
};

export default EditProduct;

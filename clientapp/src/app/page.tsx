'use client';

import CreateProduct from './product/createproduct';
import { useState } from 'react';
import { Product } from './interfaces/product';

const Home: React.FC = () => {
    const [products, setProducts] = useState<Product[]>([]);
    return (
        <div className="container">
                    <br/>
            <CreateProduct
                showCreateForm={true} // Always true to show the form
                setShowCreateForm={() => {}} // No-op function as form is always visible
                setProducts={setProducts}
            />
                    <br/>
                    <br/>
        </div>
    );
};

export default Home;

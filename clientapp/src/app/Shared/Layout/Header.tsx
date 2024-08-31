import Link from 'next/link';
import styles from './header.module.css'; 

const Header: React.FC = () => {
    return (

        <header className={styles.header}>
            <h1>Product Listing</h1>
            <nav>
                <ul>
                    <li>
                        <Link href="/">Home</Link>
                    </li>
                    <li>
                        <Link href="/product">Products</Link>
                    </li>
                </ul>
            </nav>
        </header>
    );
};

export default Header;
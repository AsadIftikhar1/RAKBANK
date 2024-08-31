import Link from 'next/link';
import styles from './footer.module.css';

const Footer: React.FC = () => {
    return (
        <footer className={styles.footer}>
            <p>&copy; 2024 RAK BANK</p>
        </footer>
    );
};

export default Footer;
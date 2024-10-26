// Initialize Stripe.js
const stripe = Stripe('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');

initialize();

// Fetch Checkout Session and retrieve the client secret
async function initialize() {
    const fetchClientSecret = async () => {
        const response = await fetch("/create-checkout-session", {
            method: "POST",
        });
        const { clientSecret } = await response.json();
        return clientSecret;
    };

    // Initialize Checkout
    const checkout = await stripe.initEmbeddedCheckout({
        fetchClientSecret,
    });

    // Mount Checkout
    checkout.mount('#checkout');
}
function redirectToCheckout(sessionId) {
    var stripe = Stripe('pk_live_51PSnwzJeAIMtIrCX3mhIgZEdPt28caqgb1XlB01iYJl90GuPy4oe7FqC0NzGKzC324p9cBvoKvfuU8hznruOm6YE00C3ZAF9GI');
    stripe.redirectToCheckout({ sessionId: sessionId });
}

cd ./src/Services/CartApi
docker build --tag tkousek/cart .
docker push tkousek/cart
kubectl delete deployment cart
kubectl delete service cart
cd /Users/tkousek/AspNetCoreMicro/ShoesOnContainers/kompose
kubectl apply -f cart-deployment.yaml
kubectl apply -f cart-service.yaml

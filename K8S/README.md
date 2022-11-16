check K8S version
    kubectl version

run platforms deploy
    kubectl apply -f <NAME OF YAML>

view deployments
    kubectl get deployments

view pods
    kubectl get pods

to delete component: NAME from get deployments
    kubectl delete deployment <NAME>
